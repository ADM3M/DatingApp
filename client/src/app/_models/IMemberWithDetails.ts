import { IMember } from "./IMember";

export interface IMemberWithDetails extends IMember {
    isFavorite: boolean;
}