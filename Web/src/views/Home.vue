<!--suppress ES6ShorthandObjectProperty -->
<template>
  <div>{{cpuLoad}}</div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  components: {
  },
  created() {
    this.connect()
  }
})
export default class Home extends Vue {
  public cpuLoad = 0;
  
  
  public connect() {
    let connection = new HubConnectionBuilder()
        .withUrl("/monitor")
        .build();
    connection.on("HardwareInfoSender", (data: any) => {
      console.log(data);
      this.cpuLoad=data;
    });

    connection.start()
        .then(() => connection.invoke("RunHardwareInfoSender"));
  }
}
</script>
