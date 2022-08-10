<template>
  <v-app>
    <v-main>
      <v-container fluid>
        <v-slide-y-transition mode="out-in">
          <v-layout column align-center>
            <router-view></router-view>
          </v-layout>
        </v-slide-y-transition>
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
      { icon: 'bubble_chart', title: 'View 1', to: '/view1' },
      { icon: 'contacts', title: 'View 1', to: '/view2' },
    ];
    public miniVariant = false;
    public right = true;
    public title = '';
    public name = '';
}
</script>
