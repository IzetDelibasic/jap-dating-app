export const MESSAGES_API = {
  BASE: 'messages',
  THREAD: (username: string) => `messages/thread/${username}`,
  DELETE: (id: number) => `messages/${id}`,
  GENERATE_MESSAGE: 'IntelligentAssistant/generate-message',
};
