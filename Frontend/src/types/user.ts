export type UserRole = "Admin" | "Doctor" | "Patient";

export interface User {
    id: string;
    email: string;
    userName: string;
    firstName?: string;
    lastName?: string;
    avatarUrl?: string;
    role: UserRole;
}