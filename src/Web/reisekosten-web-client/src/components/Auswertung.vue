<template>
  <div>
    <div v-if="bericht.reisen && bericht.reisen.length > 0">
      <table>
        <thead>
          <th>Anfang</th>
          <th>Grund</th>
          <th>Ziel</th>
          <th>Erstattung</th>
        </thead>
        <tr v-for="reise in bericht.reisen" :key="reise">
          <td>{{ reise.anfang }}</td>
          <td>{{ reise.grund }}</td>
          <td>{{ reise.zielort }}</td>
          <td>{{ reise.pauschale }} EUR</td>
        </tr>
      </table>

      <strong>Summe: {{ bericht.summe }} EUR</strong>
    </div>
    <div v-else>Keine Reisen erfasst.</div>
  </div>
</template>

<script lang="ts">
import { Vue } from 'vue-class-component';
import axios from 'axios';
import { Bericht } from '../model'

export default class Auswertung extends Vue {
  bericht : Bericht = {} as Bericht;

  created() {
    console.log('loading');

    axios.get('/auswertung')
         .then(response => {
            this.bericht = response.data as Bericht;
          });
  }
}
</script>
