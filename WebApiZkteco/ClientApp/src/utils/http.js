import axios from "axios";
// import { getCSRFToken } from "components/utils";

const http = axios.create({
  baseURL: "http://localhost:5000/api/",
  //   timeout: 1000,
  headers: {
    // "X-CSRF-Token": getCSRFToken(),
    "content-type": "application/json",
    // "content-type": "application/x-www-form-urlencoded",
  },
});

http.interceptors.request.use((req) => {
  console.log(`${req.method} ${req.url}`);
  return req;
});

http.interceptors.response.use(
  (res) => {
    console.log(res.data);
    return res.data;
  },
  (err) => {
    if (err?.response?.status === 404)
      throw new Error(`${err.config.url} not found`);
    throw err;
  }
);

export default http;
