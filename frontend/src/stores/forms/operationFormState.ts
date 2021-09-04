import { writable } from 'svelte/store';
import type { Writable } from 'svelte/store';

export type OperationPayload = {
  entries: OperationEntryPayload[];
  notes: OperationCommonNotePayload[];
};

export type OperationCommonNotePayload = string;

export type OperationEntryPayload = {
  dateTime: string;
  account: string;
  financialAccount: string;
  amount: number;
  operationEntryType: string;
  notes: OperationCommonNotePayload[];
};


export type OperationFormState = Writable<{
  entries: [{
    value: OperationEntryPayload;
  }];
  notes: [],
  isSelectionModeActive: false,
  valuesToFillOver: {
    dateTime: null,
    account: null,
    financialAccount: null,
    amount: 0.00,
    operationEntryType: null,
  }
}>;