export const MESSAGES_API = {
  BASE: 'Messages',
  THREAD: (username: string) => `Messages/thread/${username}`,
  DELETE: (id: number) => `Messages/${id}`,
  GENERATE_MESSAGE: 'IntelligentAssistant/generate-message',
};
