export const PHOTOS_API = {
  ADD_PHOTO: 'Photo/add-photo',
  APPROVE_PHOTO: (photoId: number) => `Photo/approve-photo/${photoId}`,
  DELETE_PHOTO: (photoId: number) => `Photo/delete-photo/${photoId}`,
  GET_TAGS: 'Tag',
  GET_TAGS_FOR_PHOTO: (photoId: number) => `Photo/${photoId}/tags`,
  GET_PHOTOS_BY_TAG: (tag: string) => `Tag/photos/${tag}`,
  GET_PHOTOS_BY_USER: (userId: number) => `Photo/get-photos-by-user/${userId}`,
  PHOTOS_TO_MODERATE: 'Photo/photos-to-moderate',
  REJECT_PHOTO: (photoId: number) => `Photo/reject-photo/${photoId}`,
  SET_MAIN_PHOTO: (photoId: number) => `Photo/set-main-photo/${photoId}`,
};
