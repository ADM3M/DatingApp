import {Photo} from './IPhoto';

export interface IMember {
    id: number
    name: string
    age: number
    photoUrl: string
    knownAs: string
    created: string
    lastActive: string
    gender: string
    introduction: string
    lookingFor: string
    interests: string
    city: string
    country: string
    photos: Photo[]
}