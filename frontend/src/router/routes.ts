import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/plots', // TODO: remove if homepage will be introduced
    component: () => import('layouts/MainLayout.vue'),
    children: [
      {
        path: 'plots',
        component: () => import('pages/PlotsPage.vue')
      },
      {
        path: 'auth',
        component: () => import('pages/AuthPage.vue')
      },
      {
        path: 'reg',
        component: () => import('pages/RegPage.vue')
      },
    ]
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue')
  }
];

export default routes;
