import http from "utils/http";

class UserService {
  getAll() {
    return http.get("user");
  }

  schedule(id, start, stop) {
    console.log(id, start, stop);
    return http.put(`user/schedule/${id}`, {
      start,
      stop,
    });
  }
}

export default new UserService();
