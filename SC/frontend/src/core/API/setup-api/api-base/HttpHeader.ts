export type HttpHeaderKey =
  | 'Authorization'
  | 'Content-Type'
  | 'refresh-token'
  | 'Accept-Language';

export type ContentType =
  | 'application/json'
  | 'multipart/form-data'
  | 'application/x-www-form-urlencoded';

export type HttpHeaderValue = string | string[] | number | boolean | null;

export type RawHttpHeaders = {
  [key in HttpHeaderKey]: HttpHeaderValue;
};
export class HttpHeaders {
  private headers: Map<HttpHeaderKey, HttpHeaderValue> = new Map();
  constructor(headers?: RawHttpHeaders) {
    if (headers) {
      for (const key in headers) {
        const value = headers[key as HttpHeaderKey];
        this.headers.set(key as HttpHeaderKey, value);
      }
    }
  }
  export(): RawHttpHeaders {
    const rawHeaders: RawHttpHeaders = {} as RawHttpHeaders;
    this.headers.forEach((value, key) => {
      rawHeaders[key] = value;
    });
    return rawHeaders;
  }
  set(key: HttpHeaderKey, value: HttpHeaderValue): void {
    this.headers.set(key, value);
  }
  get(key: HttpHeaderKey): HttpHeaderValue | undefined {
    return this.headers.get(key);
  }
  setContentType(value: HttpHeaderValue): void {
    this.headers.set('Content-Type', value);
  }
  getContentType(): ContentType {
    return this.headers.get('Content-Type') as ContentType;
  }
  setAuthorization(value: HttpHeaderValue): void {
    this.headers.set('Authorization', value);
  }
  getAuthorization(): HttpHeaderValue | undefined {
    return this.headers.get('Authorization');
  }
}
