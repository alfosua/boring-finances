export type ActionParams = {
  selector?: string;
  eventSuffix?: string;
};

export default function(node, params?: ActionParams) {
  const { selector, eventSuffix } = params || { };

  const trackedEventHandlers = [];
  const eventNameHandlerTemplatePairs: [string, (e: any) => void][] = [
    ['blur',   e => createHandleInputEventHandler(node, 'inputblur', e)  ],
    ['change', e => createHandleInputEventHandler(node, 'inputchange', e)],
    ['focus',  e => createHandleInputEventHandler(node, 'inputfocus', e) ],
  ];

  const baseSelectors = ['input', 'select'];
  const finalSelector = createSelectorFromMany(baseSelectors, selector);

  const childInputNodes = node.querySelectorAll(finalSelector);
  const childInputs = [].slice.call(childInputNodes);

  childInputs.forEach(child => {
    eventNameHandlerTemplatePairs.forEach(pair => {
      const [eventName, handlerTemplate] = pair;
      const finalEventName = createEventName(eventName, eventSuffix);

      child.addEventListener(finalEventName, handlerTemplate);
      trackedEventHandlers.push([finalEventName, child, handlerTemplate]);
    });
  });

  return {
    destroy() {
      trackedEventHandlers.forEach(pair => {
        const [eventName, child, handler] = pair;
        child.removeEventListener(eventName, handler);
      });
    },
  };
}

function createHandleInputEventHandler(node: HTMLElement, eventName: string, eventArgs) {
  node.dispatchEvent(new CustomEvent(eventName, createEventInitDict(node, eventArgs)));
}

function createEventInitDict(node: HTMLElement, originalEventArgs) {
  const itemKey = node.getAttribute('data-itemKey');
  const fieldKey = originalEventArgs.target.getAttribute('id');
  const value = originalEventArgs.target.value;
  const result ={
    detail: { itemKey, fieldKey, value, originalEventArgs },
  };

  return result;
}

function createEventName(baseName: string, eventSuffix: string) {
  const result = eventSuffix
    ? `${baseName}-${eventSuffix}`
    : baseName;
  return result;
}

function createSelectorFromMany(baseSelectors: string[], customSelector: string) {
  const result = baseSelectors.map(x => createSingleSelector(x, customSelector)).join(', ');
  return result;
}

function createSingleSelector(baseSelector: string, customSelector: string) {
  const result = customSelector
    ? `${baseSelector} ${customSelector}`
    : baseSelector;
  return result;
}