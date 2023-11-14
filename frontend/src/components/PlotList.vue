<script setup lang="ts">
import { defineComponent, PropType, ref } from 'vue';
import type { Ref } from 'vue';
import draggable from 'vuedraggable';
import { uid } from 'quasar';
import { PlotItem } from 'components/models';
import { all } from 'axios';

defineComponent(draggable);

const props = defineProps({
  items: {
    type: Object as PropType<PlotItem[]>,
    required: true,
  },
});

const emit = defineEmits(['createPlot', 'deletePlot', 'selectPlot']);

const dragging = ref(false);

const dragOptions = {
  animation: 300,
  group: 'description',
  disabled: false,
  ghostClass: 'ghost',
};

const selectedId = ref(props.items[0].id);

const selectItem = (item: PlotItem) => {
  selectedId.value = item.id;
  emit('selectPlot', item);
};

const createDialog = ref(false);

const newPlotName = ref('');

const validationRules = [() => !!newPlotName.value || 'Plot name is required'];

const submitCreation = () => {
  if (validationRules.reduce((acc, rule) => acc && rule() === true, true)) {
    emit('createPlot', newPlotName.value);
    createDialog.value = false;
    newPlotName.value = '';
  }
};
</script>

<template>
  <q-card flat style="width: 12rem; height: 30rem">
    <q-scroll-area style="width: 100%; height: 100%" bar-style="width: 0">
      <draggable
        :list="items as unknown[]"
        item-key="order"
        class="list-group column items-center"
        handle=".handle"
        @start="dragging = true"
        @end="dragging = false"
        v-bind="dragOptions"
        drag-class="drag"
        ghost-class="ghost"
      >
        <template #item="{ element: item }">
          <q-item
            class="items-body-content non-selectable handle q-my-xs q-py-sm q-px-sm"
            clickable
            dense
            key="element"
            @click="selectItem(item)"
            style="transition: color 0.5s; width: 98%"
            :class="selectedId === item.id ? 'text-secondary' : 'text-black'"
          >
            <q-item-section class="handle">
              <span class="text">{{ item.name }}</span>
            </q-item-section>
            <q-item-section class="row justify-center items-end">
              <q-btn
                size="md"
                class="bg-negative text-white"
                icon="delete"
                @click="$emit('deletePlot', item)"
              />
            </q-item-section>
          </q-item>
        </template>
        <template #footer>
          <q-btn
            size="md"
            class="bg-white text-secondary q-my-xs"
            color="black"
            icon="add"
            outline
            style="height: fit-content"
            @click="createDialog = true"
          />
        </template>
      </draggable>
    </q-scroll-area>
  </q-card>
  <q-dialog v-model="createDialog">
    <q-card style="min-width: 350px">
      <q-card-section>
        <div class="text-h6">Create plot</div>
      </q-card-section>

      <q-card-section class="q-pt-none">
        <q-input
          dense
          v-model="newPlotName"
          autofocus
          @keyup.enter="submitCreation"
          :rules="validationRules"
          lazy-rules
        />
      </q-card-section>

      <q-card-actions align="right" class="text-secondary">
        <q-btn flat label="Cancel" v-close-popup @click="newPlotName = ''" />
        <q-btn flat label="Create plot" @click="submitCreation" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<style scoped lang="sass">
.items-body-content
  border-radius: 15px
  border: 2px solid
  transition: border-color 0.7s, border-width 0.7s, background-color 0.5s

.ghost
  opacity: 0.1
  background-color: $secondary
</style>
