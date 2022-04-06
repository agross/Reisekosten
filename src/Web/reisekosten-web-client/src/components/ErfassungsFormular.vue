<template>
  <form @submit.prevent="submitForm">
    <label for="Anfang">Anfang</label>
    <input type="text" name="Anfang" id="Anfang" v-model="form.anfang">
    <br>

    <label for="Ende">Ende</label>
    <input type="text" name="Ende" id="Ende" v-model="form.ende">
    <br>

    <label for="Zielort">Reiseziel</label>
    <input type="text" name="Zielort" id="Zielort" v-model="form.zielort">
    <br>

    <label for="Grund">Grund</label>
    <input type="text" name="Grund" id="Grund" v-model="form.grund">
    <br>

    <button>Erfassen</button>
  </form>
</template>

<script lang="ts">
import { Vue } from 'vue-class-component';
import { Emit } from 'vue-property-decorator';
import axios from 'axios';

export default class ErfassungsFormular extends Vue {
  form =  {
    anfang: '',
    ende: '',
    zielort: '',
    grund: '',
  }

  @Emit('erfassung-erfolgt')
  public submitForm() {
    return axios.post('/erfassen', this.form);
  }
}
</script>
