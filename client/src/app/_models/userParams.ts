import { IUser } from "./IUser";

export class UserParams {
    gender = "";
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 6;
    orderBy = "lastActive";

    constructor(user: IUser) {
        
    }
}