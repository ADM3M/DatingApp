import { IUser } from "./IUser";

export class UserParams {
    gender: string;
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 10;

    constructor(user: IUser) {
        this.gender = user.gender === "female" ? "male" : "female";
    }
}