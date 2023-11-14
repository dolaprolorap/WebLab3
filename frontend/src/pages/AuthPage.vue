<template>
  <div
    class="column q-pa-md"
    style="max-width: 400px; margin: 0 auto; padding-top: 100px"
  >
    <q-form @submit="onSubmit" class="q-gutter-md">
      <q-input filled v-model="login" label="Your login" />
      <q-input filled v-model="password" label="Your password" />
      <div style="text-align: center">
        <q-btn label="Submit" type="submit" color="primary" />
      </div>
    </q-form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useQuasar } from 'quasar';
import axios from 'axios';
const login = ref('');
const password = ref('');
const backendServer = 'https://localhost:7021';
const $q = useQuasar();

const onSubmit = () => {
  axios
    .post(backendServer + '/api/Authentication/login', {
      login: login.value,
      password: password.value,
    })
    .then((response) => {
      if (response.status == 200) {
        let data = response.data;
        $q.localStorage.set('refreshToken', data.refreshToken);
        window.location.href = '/';
      } else console.log(response);
    });
  console.log(login, password);
};
</script>
