export interface User {
    id: string;
    email: string;
    userName: string;
    firstName?: string;
    lastName?: string;
    avatarUrl?: string;
}

export interface RegisterUser {
    userName: string;
    email: string;
    password: string;
}