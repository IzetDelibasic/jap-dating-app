export interface User {
  id: number;
  username?: string;
  knownAs: string;
  gender: string;
  token: string;
  photoUrl?: string;
  roles: string[];
}
