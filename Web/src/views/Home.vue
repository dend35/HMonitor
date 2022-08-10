<!--suppress ES6ShorthandObjectProperty -->
<template>
  <div>
    <v-card min-width="900" dark style="padding: 0px 20px 20px;">
      <v-card-title>
      </v-card-title>
<!--      <div style="width: 600px">{{ computer }}</div>-->
      <v-row>
        <v-col cols="6">
          <div class="mt-3">
            <span>CPU: {{ Math.round(computer.cpuLoad) }}%</span>
            <v-progress-linear
                :value="computer.cpuLoad"
                height="20"
                rounded
            >
              {{ Math.round(computer.cpuLoad) }}%
            </v-progress-linear>
            <v-progress-linear
                :value="computer.cpuTemp"
                color="#ee204d"
                height="20"
                rounded
            >
              {{ Math.round(computer.cpuTemp) }}°
            </v-progress-linear>
          </div>
          <div class="mt-3">
            <span>RAM: {{ Math.round(computer.ramLoad) }}%</span>
            <v-progress-linear
                :value="computer.ramLoad"
                height="20"
                rounded
            >
              {{ Math.round(computer.ramUsed*100)/100 }} Gb
            </v-progress-linear>
          </div>
          <div class="mt-3">
            <span>NET: {{  }}</span>
            <br>
            <span style="width: 200px">
              D: {{Math.round(computer.downSpeed/1024/1024*100)/100}} Mbs
            </span>
            <span>
              {{Math.round(computer.downBytes)}} Gb
            </span>
            <v-divider></v-divider>
            <span>
              U: {{Math.round(computer.upSpeed/1024/1024*100)/100}} Mbs
               {{Math.round(computer.upBytes)}} Gb
            </span>
          </div>
        </v-col>
        <v-col cols="6">
          <div class="mt-3">
            <span>GPU: {{ Math.round(computer.gpuLoad) }}%</span>
            <v-progress-linear
                :value="computer.gpuLoad"
                height="20"
                rounded
            >
              {{ Math.round(computer.gpuLoad) }}%
            </v-progress-linear>
            <v-progress-linear
                :value="computer.gpuTemp"
                color="#ee204d"
                height="20"
                rounded
            >
              {{ Math.round(computer.gpuTemp) }}°
            </v-progress-linear>
          </div>
          <div v-if="computer.fps > 0" class="mt-3">
            FPS: {{computer.fps}}
          </div>
        </v-col>
      </v-row>
    </v-card>


  </div>
</template>

<script lang="ts">
import {Component, Vue} from 'vue-property-decorator';
import {HubConnectionBuilder} from '@microsoft/signalr';

@Component({
  components: {}
})
export default class Home extends Vue {
  public computer = {
    cpuLoad: 0,
    gpuLoad: 0,
    gpuTemp: 0,
    ramLoad: 0,
    ramAvailable: 0,
    ramUsed: 0,
    cpuTemp: 0,
    fps: 0,
    downSpeed: 0,
    upSpeed: 0,
    downBytes: 0,
    upBytes: 0,
  };

  async created() {
    let connection = new HubConnectionBuilder()
        .withUrl("/monitor")
        .build();
    connection.on("HardwareInfoSender", (data: any) => {
      this.computer = data;
    });
    await this.connect(connection)
  }

  async connect(connection:any) {
    try {
      await connection.start()
      await connection.invoke("RunHardwareInfoSender")
    }catch (e){
      await new Promise(resolve => setTimeout(resolve, 1000));
      await this.connect(connection)
    }
  }
}
</script>

<style scoped>

</style>
