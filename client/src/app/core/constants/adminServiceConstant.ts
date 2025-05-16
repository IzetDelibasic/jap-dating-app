export const ADMIN_API = {
  USERS_WITH_ROLES: 'admin/users-with-roles',
  EDIT_ROLES: (username: string, roles: string[]) =>
    `admin/edit-roles/${username}?roles=${roles}`,
  PHOTOS_TO_MODERATE: 'admin/photos-to-moderate',
  APPROVE_PHOTO: (photoId: number) => `admin/approve-photo/${photoId}`,
  REJECT_PHOTO: (photoId: number) => `admin/reject-photo/${photoId}`,
};
