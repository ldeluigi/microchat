export interface Message {
  id: string,
  chatId: string,
  text: string,
  sendTime: Date,
  edited: boolean,
  sender: string,
}