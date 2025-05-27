export const MEMBERS_API = {
  BASE: 'user',
  BY_USERNAME: (username: string) => `user/${username}`,
  UPDATE: 'user',
  SET_MAIN_PHOTO: (photoId: number) => `user/set-main-photo/${photoId}`,
  DELETE_PHOTO: (photoId: number) => `user/delete-photo/${photoId}`,
  GET_TAGS: 'tag',
  GET_TAGS_FOR_PHOTO: (photoId: number) => `photo/${photoId}/tags`,
  GET_PHOTOS_BY_TAG: (tag: string) => `tag/photos/${tag}`,
};
