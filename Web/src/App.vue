<template>
  <v-app>
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
<style scoped>
.firefly{
  position: fixed;
  left: 50%;
  top: 50%;
  width: 0.4vw;
  height: 0.4vw;
  margin: -0.2vw 0 0 9.8vw;
  animation: ease 200s alternate infinite;
  pointer-events: none;
}

</style>
