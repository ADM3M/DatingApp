import { LikeState } from "../enums/LikeState"

export const getLikeMessage = (state: LikeState) => {
    if (state === LikeState.Liked) {
        return 'User was added to favorites';
    }

    if (state === LikeState.Unliked) {
        return 'User was removed from favorites';
    }

    return 'User was either added or removed from favorites';
}