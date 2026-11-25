export interface DoctorDto {
  id: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  specialty: string;
  roomNumber: number;
  avatarUrl?: string | null;
}