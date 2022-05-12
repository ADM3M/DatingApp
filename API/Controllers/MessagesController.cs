using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO createMessageDTO)
        {
            var username = User.GetUserName();

            if (username == createMessageDTO.RecipientName.ToLower())
            {
                return BadRequest("You can't send messages to yourself");
            }

            var sender = await _unitOfWork.UserReposiroty.GetUserByUserNameAsync(username);
            var recipient = await _unitOfWork.UserReposiroty.GetUserByUserNameAsync(createMessageDTO.RecipientName);

            if (recipient is null)
            {
                return NotFound();
            }

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderName = sender.UserName,
                RecipientName = recipient.UserName,
                Content = createMessageDTO.Content,
            };

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete())
            {
                return Ok(_mapper.Map<MessageDTO>(message));
            }

            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForUser([FromQuery] 
            MessageParams messageParams)
        {
            messageParams.Name = User.GetUserName();

            var messages = await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, 
                messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUserName();
            var message = await _unitOfWork.MessageRepository.GetMessage(id);

            if (message.SenderName != username && message.RecipientName != username)
            {
                return Unauthorized();
            }

            if (username == message.SenderName)
            {
                message.SenderDeleted = true;
            }
            else if (username == message.RecipientName)
            {
                message.RecipientDeleted = true;
            }

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _unitOfWork.MessageRepository.DeleteMessage(message);
            }

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("Problem deleting the message");
        }

    }
}