'use client'

import { useMemo } from 'react'
import { useForm } from 'react-hook-form'
import { z } from 'Zod'
import { zodResolver } from '@hookform/resolvers/zod'

const inputsSchema = z.object({
  initialInput: z.number(),
  inputIncreaseRate: z.number(),
  cycles: z.number(),
  growthPercentage: z.number(),
})

export type Inputs = z.infer<typeof inputsSchema>

export default function Page() {
  const { register, watch } = useForm<Inputs>({
    defaultValues: {
      initialInput: 10,
      inputIncreaseRate: 10,
      cycles: 10,
      growthPercentage: 10,
    },
    resolver: zodResolver(inputsSchema),
  })
  const inputs = watch()
  const calcs = useMemo(() => makeCalcs({ ...inputs }), [inputs])
  return (
    <div>
      <h1>Exponential Grow</h1>
      <div className='flex flex-row gap-4'>
        <Field label='Initial Input' register={register('initialInput', { setValueAs: (value) => Number(value) })} />
        <Field
          label='Input Increase Rate'
          register={register('inputIncreaseRate', { setValueAs: (value) => Number(value) })}
        />
        <Field label='Cycles' register={register('cycles', { setValueAs: (value) => Number(value) })} />
        <Field
          label='Growth Percentage'
          register={register('growthPercentage', { setValueAs: (value) => Number(value) })}
        />
      </div>
      <div className='flex flex-row gap-4'>
        <ComputedField label='Initial Input' value={likeCurrency(calcs.initialInput)} />
        <ComputedField label='Additional Input' value={likeCurrency(calcs.additionalInput)} />
        <ComputedField label='Final Input' value={likeCurrency(calcs.finalInput)} />
        <ComputedField label='Earnings' value={likeCurrency(calcs.totalEarnings)} />
        <ComputedField label='Total' value={likeCurrency(calcs.total)} />
      </div>
      <div>
        <h2>Monthly Snapshots</h2>
        <table className='border-black border-2'>
          <thead>
            <tr>
              <MonthlySnapshotHeader value='Cycle' />
              <MonthlySnapshotHeader value='Total Before' />
              <MonthlySnapshotHeader value='Month Earnings' />
              <MonthlySnapshotHeader value='Additional Input Before' />
              <MonthlySnapshotHeader value='Total Earnings Before' />
              <MonthlySnapshotHeader value='Total' />
            </tr>
          </thead>
          <tbody>
            {calcs.monthlySnapshots.map((snapshot, index) => (
              <tr key={index}>
                <MonthlySnapshotNumberCell value={index + 1} />
                <MonthlySnapshotAmountCell value={likeCurrency(snapshot.totalBefore)} />
                <MonthlySnapshotAmountCell value={likeCurrency(snapshot.cycleEarnings)} />
                <MonthlySnapshotAmountCell value={likeCurrency(snapshot.additionalInputBefore)} />
                <MonthlySnapshotAmountCell value={likeCurrency(snapshot.totalEarningsBefore)} />
                <MonthlySnapshotAmountCell value={likeCurrency(snapshot.total)} />
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

function MonthlySnapshotHeader({ value }: { value: any }) {
  return <th className='text-center px-4'>{value}</th>
}

function MonthlySnapshotNumberCell({ value }: { value: any }) {
  return <td className='text-center font-mono'>{value}</td>
}

function MonthlySnapshotAmountCell({ value }: { value: any }) {
  return <td className='text-right font-mono'>{value.toFixed(2)}</td>
}

function likeCurrency(num: number) {
  return Math.round(num * 1e2) / 1e2
}

function Field({ label, register }: { label: string; register: any }) {
  return (
    <div className='flex flex-col'>
      <label className='text-sm'>{label}</label>
      <input {...register} className='border-black border-2 rounded-md w-20' />
    </div>
  )
}

function ComputedField({ label, value }: { label: string; value: any }) {
  return (
    <div className='flex flex-col'>
      <label className='text-sm'>{label}</label>
      <span className='border-black border-2 rounded-md w-30'>{value}</span>
    </div>
  )
}

function makeCalcs({ initialInput, inputIncreaseRate, cycles, growthPercentage }: Inputs) {
  const cycleSnapshots = []
  let totalEarnings = 0
  let additionalInput = 0
  let total = initialInput
  for (let i = 0; i < cycles; i++) {
    const totalBefore = total
    const totalEarningsBefore = totalEarnings
    const additionalInputBefore = additionalInput
    const isLastCycle = i + 1 == cycles
    const cycleEarnings = (total * growthPercentage) / 100
    totalEarnings += cycleEarnings
    if (!isLastCycle) {
      total += cycleEarnings + inputIncreaseRate
      additionalInput += inputIncreaseRate
    } else {
      total += cycleEarnings
    }
    cycleSnapshots.push({
      cycleEarnings,
      additionalInput,
      additionalInputBefore,
      totalEarnings,
      totalEarningsBefore,
      total,
      totalBefore,
    })
  }
  const finalInput = initialInput + additionalInput
  const result = {
    initialInput,
    additionalInput,
    finalInput,
    totalEarnings,
    total,
    monthlySnapshots: cycleSnapshots,
  }
  return result
}
