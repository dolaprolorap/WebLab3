<template>
  <div
    class="column q-pa-md"
    style="max-width: 400px; margin: 0 auto; padding-top: 100px"
  >
    <q-form @submit="onSubmit" class="q-gutter-md">
      <q-input filled v-model="login" label="Login" />
      <q-input
        v-model="password"
        filled
        :type="isPwd ? 'password' : 'text'"
        label="Password"
      >
        <template v-slot:append>
          <q-icon
            :name="isPwd ? 'visibility_off' : 'visibility'"
            class="cursor-pointer"
            @click="isPwd = !isPwd"
          />
        </template>
      </q-input>
      <q-btn align="center" label="Submit" type="submit" color="primary" />
    </q-form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useQuasar } from 'quasar';
import axios from 'axios';
import { useRouter } from 'vue-router';

const login = ref('');
const password = ref('');

const isPwd = ref(true);

const $q = useQuasar();
const $router = useRouter();

const onSubmit = () => {
  axios
    .post(process.env.API + '/api/Authentication/login', {
      login: login.value,
      password: password.value,
    })
    .then((response) => {
      if (response.status == 200) {
        const data = response.data;
        $q.localStorage.set('token', data.token);
        $q.localStorage.set('refreshToken', data.refreshToken);
        window.location.href = '/';
      } else console.log(response);
    });
  console.log(login, password);
};
</script>
