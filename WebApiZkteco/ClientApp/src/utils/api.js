import axios from "axios";
// import { getCSRFToken } from "components/utils";

const api = axios.create({
  baseURL: "http://localhost:5000/api/",
  //   timeout: 1000,
  headers: {
    // "X-CSRF-Token": getCSRFToken(),
  },
});

api.interceptors.request.use((req) => {
  console.log(`${req.method} ${req.url}`);
  return req;
});

api.interceptors.response.use(
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

export default api;
