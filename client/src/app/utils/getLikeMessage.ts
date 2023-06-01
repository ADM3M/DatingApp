import { LikeState } from "../enums/LikeState"

export const getLikeMessage = (state: LikeState) => {
    if (state === LikeState.Liked) {
        return 'Пользователь добавлен в избранное';
    }

    if (state === LikeState.Unliked) {
        return 'Пользователь больше не избранный';
    }

    return 'User was either added or removed from favorites';
}