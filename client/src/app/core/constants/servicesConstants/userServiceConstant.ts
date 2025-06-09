export const USER_API = {
  BASE: 'user',
  BY_USERNAME: (username: string) => `user/${username}`,
  UPDATE: 'user',
  SET_MAIN_PHOTO: (photoId: number) => `user/set-main-photo/${photoId}`,
  DELETE_PHOTO: (photoId: number) => `user/delete-photo/${photoId}`,
  USERS_WITH_ROLES: 'user/users-with-roles',
  EDIT_ROLES: (username: string, roles: string[]) =>
    `user/edit-roles/${username}?roles=${roles}`,
};
