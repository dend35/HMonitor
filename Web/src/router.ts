import Vue from 'vue';
import Router from 'vue-router';

import Home from './views/Home.vue';

Vue.use(Router);

const routes = [
  { path: '/', component: Home, props: { name: 'Vuetify' } },
  { path: '*', redirect: '/' },
];

export default new Router ({
    mode: 'history',
    linkActiveClass: 'active',
    routes,
});
