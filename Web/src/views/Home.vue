<!--suppress ES6ShorthandObjectProperty -->
<template>
  <div style="display: flex; flex-direction: column; align-items: center">
    <v-card :color="panelColor" min-width="100%" dark style="padding: 0 20px 20px;">
      <!--      <div style="width: 600px">{{ computer }}</div>-->
      <v-row id="Metrics">
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
                :color="computeTempColor(computer.cpuTemp)"
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
              {{ Math.round(computer.ramUsed * 100) / 100 }} Gb
            </v-progress-linear>
          </div>
          <div class="mt-3">
            <span>NET: {{  }}</span>
            <br>
            <span style="width: 200px">
              D: {{ Math.round(computer.downSpeed / 1024 / 1024 * 100) / 100 }} Mbs
            </span>
            <span>
              {{ Math.round(computer.downBytes * 100) / 100 }} Gb
            </span>
            <v-divider></v-divider>
            <span>
              U: {{ Math.round(computer.upSpeed / 1024 / 1024 * 100) / 100 }} Mbs
               {{ Math.round(computer.upBytes * 100) / 100}} Gb
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
                :color="computeTempColor(computer.gpuTemp)"
                height="20"
                rounded
            >
              {{ Math.round(computer.gpuTemp) }}°
            </v-progress-linear>
          </div>
          <div v-if="computer.fps > 0" class="mt-3">
            FPS: {{ Math.round(computer.fps) }}
          </div>
        </v-col>
      </v-row>
    </v-card>
    <v-row id="Buttons">

    </v-row>
    <v-row></v-row>
    <v-card dark :color="panelColor" elevation="10" style="position: absolute; bottom: 10px; right: 10px; ">
      <div>
        <v-btn class="ma-1" height="60" @click="controlHub.invoke(`Prev`)">
          <v-icon>mdi-skip-backward</v-icon>
        </v-btn>
        <v-btn class="ma-1" height="60" @click="controlHub.invoke(`Next`)">
          <v-icon>mdi-skip-forward</v-icon>
        </v-btn>
      </div>
      <v-btn class="ma-1" style="width: 95%" height="60" @click="controlHub.invoke(`Play`)">
        <v-icon>mdi-play-pause</v-icon>
      </v-btn>
    </v-card>
    <v-card dark :color="panelColor" style="position: absolute; bottom: 10px; left: 10px">
      <v-btn class="ma-1" height="50" width="50" @click="controlHub.invoke(`PlayDota`)"> 
        <v-img 
            src="@/assets/img/dota.png"
            width="50"
            height="50"
        >
        </v-img >
      </v-btn>
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
  panelColor = "rgb(0, 0, 0, 0.5)"
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
  monitorHub = new HubConnectionBuilder()
      .withUrl("/monitor")
      .build();
  controlHub = new HubConnectionBuilder()
      .withUrl("/control")
      .build();

  async created() {
    setTimeout(() => {window.location.reload();}, 60 * 60 * 1000)
    this.monitorHub.on("HardwareInfoSender", (data: any) => {
      this.computer = data;
    });
    this.monitorHub.onclose(async () => {
      await this.connect(this.monitorHub);
    });
    this.controlHub.onclose(async () => {
      await this.connect(this.controlHub);
    });
    await this.connect(this.monitorHub, (connection: any) => connection.invoke("RunHardwareInfoSender"))
    await this.connect(this.controlHub)
  }

  async connect(connection: any, action: any = null) {
    try {
      await connection.start()
      if (action !== null)
        await action(connection)
    } catch (e) {
      await new Promise(resolve => setTimeout(resolve, 1000));
      await this.connect(connection, action)
    }
  }
  computeTempColor(value:Number){
    let alertColor = "#ee204d"
    let color = "#009900"
    if(value>80)
      return alertColor
    else 
      return color
  }
}
</script>

<style scoped>

</style>
