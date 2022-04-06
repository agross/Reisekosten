<template>
  <div>
    <h1>Reisekostenerfassung</h1>

    <ErfassungsFormular @erfassung-erfolgt="forceRerender" />

    <h2>Auswertung</h2>
    <Auswertung :key="auswertungKey" />
  </div>
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component';
import HelloWorld from './components/HelloWorld.vue';
import ErfassungsFormular from './components/ErfassungsFormular.vue'
import Auswertung from './components/Auswertung.vue'

@Options({
  components: {
    HelloWorld,
    ErfassungsFormular,
    Auswertung,
  },
})
export default class App extends Vue {
  auswertungKey = 0;

  forceRerender() {
    this.auswertungKey += 1;
  }
}
</script>

<style>
#app {
  font-family: Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: left;
  margin: auto 3em;
  color: #2c3e50;
}
</style>
