import axios from 'axios';
import { LocalStorage } from 'quasar';

const getNewTokens = () => {

  if (!LocalStorage.has('token')) throw Error('Not authorized');

  axios
    .post(process.env.API + '/api/Token/refresh', {
      token: LocalStorage.getItem('token'),
      refreshToken: LocalStorage.getItem('refreshToken'),
    })
    .then((response) => {
      const data = response.data;
      LocalStorage.set('token', data.token);
      LocalStorage.set('refreshToken', data.refreshToken);
    });
};
export const authPost = (url: string, data: object) => {
  getNewTokens();

  if (!LocalStorage.has('token')) {
    throw Error('Not authorized');
  }

  return axios.post(url, data,{
    headers: {
      Authorization: 'Bearer ' + LocalStorage.getItem('token'),
    }
  });
};

export const authGet = (url: string) => {
  getNewTokens();

  if (!LocalStorage.has('token')) {
    throw Error('Not authorized');
  }

  return axios.get(url, {
    headers: {
      Authorization: 'Bearer ' + LocalStorage.getItem('token'),
    }
  });
};
