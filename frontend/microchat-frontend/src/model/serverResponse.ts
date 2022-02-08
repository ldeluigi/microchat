export interface Response<DataType> {
  data: DataType;
}

export interface ResponsePaginate<DataType> extends Response<DataType> {
  meta: {
    pageIndex: number,
    pageSize: number,
    rowCount: number,
    pageCount: number
  },
}
