export const USER_API = {
  BASE: 'Users',
  BY_USERNAME: (username: string) => `Users/${username}`,
  UPDATE: 'Users',
  USERS_WITH_ROLES: 'Users/users-with-roles',
  EDIT_ROLES: (username: string, roles: string[]) =>
    `Users/edit-roles/${username}?roles=${roles}`,
};
