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
