const autoPreprocess = require('svelte-preprocess');

module.exports = {
  preprocess: autoPreprocess({
    scss: { includePaths: ['src/styles'] },
    postcss: { plugins: [require('autoprefixer')()] }
  }),
};