<script setup lang="ts">
import { reactive, ref } from 'vue';
import axios from 'axios';
import { authGet, authPost } from 'src/utils';
import LineChart from 'components/charts/LineChart.vue';

const apiKey = ref('');

const apiUrl = ref('https://api.weatherapi.com/v1/current.json?key=ddb01da34f7e4c5f9eb195438231411&q=Alabama&aqi=no');

const fieldPath = ref('.current.temp_c');

const jsonDialog = ref(false);
let testJson = {};

const fetchInterval = ref(1);

const fetchAmount = ref(10);

const dataLoaded = ref(false);

let realId = '';

let timer = null;

const plotX = ref([] as number[])

const plotY = ref([] as number [])

const props = defineProps({
  plotId: String,
  plotName: String,
});

const testApi = () => {
  axios
    .post(process.env.API + '/api/Listener/knock', {
      url: apiUrl.value,
      key: apiKey.value,
    })
    .then((response) => {
      let data = response.data;
      testJson = data;
      jsonDialog.value = true;
    })
    .catch((error) => {
      console.log(error);
    });
};

const startFetch = () => {
  authPost(process.env.API + '/api/Plot', {
    name: props.plotName,
  })
    .then((response) => {
      realId = response.data.id;
      authPost(process.env.API + '/api/Listener', {
        url: apiUrl.value,
        key: apiKey.value,
        count: fetchAmount.value,
        sleep: fetchInterval.value,
        plotId: realId,
      }).then(() => {
        timer = setInterval(fetchData, fetchInterval.value * 1000);
      });
    })
    .catch((error) => {
      console.log(error);
    });
};

const fetchData = () => {
  authGet(process.env.API + '/api/Listener/' + realId).then((response) => {
    let fields = fieldPath.value.split('.').slice(1);

    plotY.value = response.data.map((obj: { data: string }) =>
      fields.reduce(
        (acc: object, field: string) => acc[field as keyof object],
        JSON.parse(obj.data)
      )
    );

    plotX.value = [...Array(plotY.value.length).keys()]

    dataLoaded.value = true;
  });
};
</script>

<template>
  <transition enter-active-class="animated fadeIn">
    <q-card flat>
      <q-card-section class="text-h6 text-center q-pa-none"
        >{{ plotName }}
      </q-card-section>
      <q-card-section class="row justify-between">
        <q-input
          v-model="apiUrl"
          hint="URL"
          placeholder="https://api.example.com/data"
          color="secondary"
          outlined
          dense
        />
        <q-input
          v-model="apiKey"
          hint="API key"
          placeholder="s4mpl3ap1key1234"
          color="secondary"
          outlined
          dense
        />
        <q-input
          v-model="fieldPath"
          hint="Field"
          placeholder=".current.temp_c"
          color="secondary"
          outlined
          dense
        />
        <q-btn @click="testApi">Test</q-btn>
      </q-card-section>
      <q-card-section class="q-pb-none">
        <div class="plot">
          <LineChart v-if='dataLoaded' :axes='{x: plotX, y: plotY}'/>
        </div>
      </q-card-section>
      <q-card-section class="row justify-between">
        <q-input
          dense
          v-model="fetchInterval"
          :rules="[
            () =>
              (!isNaN(+fetchInterval) && fetchInterval > 0) ||
              'Not a positive number',
          ]"
          lazy-rules
          outlined
          label="Interval"
        />
        <q-input
          dense
          v-model="fetchAmount"
          :rules="[
            () =>
              (String(fetchAmount) === String(parseInt(String(fetchAmount))) &&
                fetchAmount > 0) ||
              'Not a natural number',
          ]"
          lazy-rules
          outlined
          label="Amount"
        />
        <q-btn @click="startFetch">Start fetching</q-btn>
      </q-card-section>

      <q-dialog v-model="jsonDialog" seamless position="right">
        <q-card style="width: 25rem">
          <q-card-actions class="justify-between q-pa-lg">
            <div class="text-h6">Answer JSON</div>
            <q-btn flat icon="close" color="secondary" v-close-popup />
          </q-card-actions>

          <q-separator />

          <q-card-section style="max-height: 30rem" class="scroll">
            <pre>{{ testJson }}</pre>
          </q-card-section>
        </q-card>
      </q-dialog>
    </q-card>
  </transition>
</template>

<style scoped lang="sass">
.plot
  height: 20rem
  width: 40rem
  border: solid $secondary
</style>
