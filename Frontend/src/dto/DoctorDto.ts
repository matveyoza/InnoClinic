export interface DoctorDto {
  id: string;
  firstName: string | null;
  lastName: string | null;
  userName: string | null;
  email: string;
  specialty: string;
  roomNumber: number;
  avatarUrl?: string | null;
}