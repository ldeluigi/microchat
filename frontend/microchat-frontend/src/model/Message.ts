export interface Message {
  id: string,
  text: string,
  sendTime?: Date,
  lastEditTime?: Date,
  sender: string,
  editText(newText: string): void 
}