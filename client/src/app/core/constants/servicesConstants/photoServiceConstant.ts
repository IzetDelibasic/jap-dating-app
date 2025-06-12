export const PHOTOS_API = {
  ADD_PHOTO: 'photo/add-photo',
  APPROVE_PHOTO: (photoId: number) => `photo/approve-photo/${photoId}`,
  GET_TAGS: 'tag',
  GET_TAGS_FOR_PHOTO: (photoId: number) => `photo/${photoId}/tags`,
  GET_PHOTOS_BY_TAG: (tag: string) => `tag/photos/${tag}`,
  GET_PHOTOS_BY_USER: (userId: number) => `photo/get-photos-by-user/${userId}`,
  PHOTOS_TO_MODERATE: 'photo/photos-to-moderate',
  REJECT_PHOTO: (photoId: number) => `photo/reject-photo/${photoId}`,
};
