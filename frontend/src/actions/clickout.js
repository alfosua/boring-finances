export default function(node) {
  
  function handleClick(event) {
    if (node != event.target && !node.contains(event.target) && !node.defaultPrevented) {
      node.dispatchEvent(new CustomEvent('clickout', {...event, relatedTarget: node}));
    }
  }

  document.addEventListener('click', handleClick);

  return {
    destroy() {
      document.removeEventListener('click', handleClick)
    }
  }
}