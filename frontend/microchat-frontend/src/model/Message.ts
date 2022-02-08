export interface Message {
  id: string,
  chatId: string,
  text: string,
  sendTime: Date,
  edited: boolean,
  viewed: boolean,
  sender: string,
}

export interface MessageDto {
  id: string,
  chat: string,
  text: string,
  sendTime: string,
  lastEditTime: string,
  viewed: boolean,
  sender: string
}

export function toMessage(dto: MessageDto): Message {
  const message: Message = {
    id: dto.id,
    chatId: dto.chat,
    text: dto.text,
    sendTime: new Date(Date.parse(dto.sendTime)),
    edited: dto.lastEditTime? true : false,
    viewed: dto.viewed,
    sender: dto.sender,
  }
  return message;
}