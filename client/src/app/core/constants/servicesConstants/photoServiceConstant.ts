export const PHOTOS_API = {
  PHOTOS_TO_MODERATE: 'photo/photos-to-moderate',
  APPROVE_PHOTO: (photoId: number) => `photo/approve-photo/${photoId}`,
  REJECT_PHOTO: (photoId: number) => `photo/reject-photo/${photoId}`,
  GET_TAGS: 'tag',
  GET_TAGS_FOR_PHOTO: (photoId: number) => `photo/${photoId}/tags`,
  GET_PHOTOS_BY_TAG: (tag: string) => `tag/photos/${tag}`,
};
