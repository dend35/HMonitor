<template>
  <v-app class="v-app">
    <v-main>
      <v-container fluid style="overflow: hidden;">
        <router-view></router-view>
      </v-container>
    </v-main>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class AppComponent extends Vue {
    public clipped = false;
    public drawer = false;
    public fixed = false;
    public items = [
      { icon: 'home', title: 'Home', to: '/' },
    ];
    public miniVariant = false;
    public right = true;
    public title = '';
    public name = '';
}
</script>
