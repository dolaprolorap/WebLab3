<script setup lang="ts">
import { ref } from 'vue';
import type { Ref } from 'vue';
import PlotContainer from 'components/PlotContainer.vue';
import { uid } from 'quasar';
import { PlotItem } from 'components/models';
import PlotList from 'components/PlotList.vue';

const plots: Ref<PlotItem[]> = ref([
  { id: uid(), order: 0, name: 'github stars' },
  { id: uid(), order: 1, name: 'population of Alabama' },
  { id: uid(), order: 2, name: 'pudge arcane price' },
]);

const createPlot = (name: string) => {
  plots.value.push({
    id: uid(),
    order: plots.value.length,
    name: name,
  });
};

const deletePlot = (item: PlotItem) => {
  plots.value = plots.value.filter((plot) => plot.id !== item.id);
};

const selectedPlot = ref(plots.value[0].id);

const selectPlot = (item: PlotItem) => {
  selectedPlot.value = item.id;
}
</script>

<template>
  <q-page class="row justify-center items-center">
    <q-card class='row justify-between items-start q-my-xs no-wrap'>
      <q-card-section class='q-ma-sm'>
        <PlotList
          :items="plots"
          @delete-plot="deletePlot"
          @create-plot="createPlot"
          @select-plot="selectPlot"
        />
        </q-card-section>
      <q-separator vertical/>
      <q-card-section class='q-pa-none q-ma-sm' style='overflow: hidden; max-height: 33rem'>
        <PlotContainer
          v-for="plot in plots"
          :key="plot.id"
          v-show="plot.id === selectedPlot"
          :plot-id='plot.id'
          :plot-name='plot.name'
        />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<style scoped lang="sass"></style>
