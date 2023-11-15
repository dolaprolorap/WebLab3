<script setup lang="ts">
import { defineProps } from 'vue'
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
} from 'chart.js'
import { Line } from 'vue-chartjs'

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend)

type Axes = {
  x: string[] | number[]
  y: number[]
}
const props = defineProps<{
  axes: Axes
}>()

const data = {
  labels: props.axes.x,
  datasets: [
    {
      label: 'Data One',
      backgroundColor: '#f87979',
      data: props.axes.y
    }
  ]
}

const options = {
  responsive: true,
  maintainAspectRatio: false
}
</script>

<template>
  <Line :data="data" :options="options" />
</template>

