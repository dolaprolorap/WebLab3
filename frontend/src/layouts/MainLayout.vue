<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar class="q-px-xl q-py-md row justify-between items-baseline">
        <q-toolbar-title shrink class="non-selectable col">
          <router-link to="/" class="nav-item site-title">
            D'SKY PLOT
          </router-link>
        </q-toolbar-title>
        <div class="col-5 row justify-between">
          <router-link
            to="/plots"
            class="pages-nav nav-item page-title non-selectable"
          >
            plots
          </router-link>
          <router-link
            to="/market"
            class="pages-nav nav-item page-title non-selectable"
          >
            market
          </router-link>
          <router-link
            to="/profile"
            class="pages-nav nav-item page-title non-selectable"
            v-if="authorized"
          >
            profile
          </router-link>

          <a
            href="#"
            class="pages-nav nav-item page-title"
            v-if="authorized"
            @click="LogOut"
          >
            exit
          </a>
          <router-link
            to="/auth"
            class="pages-nav nav-item page-title"
            v-if="!authorized"
          >
            auth
          </router-link>
          <router-link
            to="/reg"
            class="pages-nav nav-item page-title"
            v-if="!authorized"
          >
            reg
          </router-link>
        </div>
      </q-toolbar>
    </q-header>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { useQuasar } from 'quasar';

const $q = useQuasar();
const authorized = $q.localStorage.getItem('refreshToken') !== null;
const LogOut = () => {
  $q.localStorage.clear();
  window.location.href = '/';
};
</script>

<style scoped lang="sass">
.site-title
  font-family: "Josefin Sans", sans-serif
  font-weight: bold
  font-size: x-large

.page-title
  font-family: "Ubuntu Regular", sans-serif
  font-size: medium

.nav-item
  text-decoration: none
  color: black
  display: block
  position: relative

  &:after
    position: absolute
    bottom: 0
    left: 0
    right: 0
    margin: auto
    width: 0
    content: '.'
    color: transparent
    background: black
    height: 1px
    transition: all 0.3s

  &:hover:after
    width: 100%
</style>
