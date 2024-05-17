const path = require('path');
const webpack = require('webpack');
const fs = require('fs');

const OptimizeCSSAssetsPlugin = require('css-minimizer-webpack-plugin');
const TerserJSPlugin = require('terser-webpack-plugin');
const { TypedCssModulesPlugin } = require('typed-css-modules-webpack-plugin');

const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const MiniCssExtractPluginConfig = new MiniCssExtractPlugin({
    filename: 'smartplaylist.2.5.1.4850.css',
});
const outDir = path.join(__dirname, '../backend/SmartPlaylist/Configuration');

module.exports = {
    mode: 'production',
    plugins: [
        MiniCssExtractPluginConfig,
        new TypedCssModulesPlugin({
            globPattern: 'src/**/*.css',
        }),
        new webpack.NormalModuleReplacementPlugin(/./, function (resource) {
            if (!resource.context.includes('emby')) {
                const component = resource.request.match(/[^\/]+$/)[0];
                const isFileExists = fs.existsSync(
                    path.resolve(__dirname, `./src/emby/components/${component}.tsx`),
                );
                if (isFileExists) {
                    resource.request = `~/emby/components/${component}`;
                }
                const isFileExists2 = fs.existsSync(
                    path.resolve(__dirname, `./src/emby/${component}.ts`),
                );
                if (isFileExists2) {
                    resource.request = `~/emby/${component}`;
                }
            }
        }),
    ],
    entry: ['./src/index'],

    optimization: {
        minimizer: [
            new TerserJSPlugin({
                parallel: true,
                parallel: 4,
                extractComments: false,
                terserOptions: { output: { comments: false } },
            }),
            new OptimizeCSSAssetsPlugin({}),
        ],
        splitChunks: false,
    },

    resolve: {
        extensions: ['.ts', '.tsx', '.js', '.css'],
        alias: {
            '~/*': path.resolve(__dirname, './src'),
            '~/common': path.resolve(__dirname, './src/common'),
            '~/app': path.resolve(__dirname, './src/app'),
            '~/emby': path.resolve(__dirname, './src/emby'),
        },
    },

    output: {
        path: outDir,
        filename: 'smartplaylist.2.5.1.4850.js',
    },

    module: {
        rules: [
            {
                test: /\.css$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: {
                            importLoaders: 1,
                            sourceMap: false,
                        },
                    },
                ],
            },
            {
                test: /\.ts(x?)$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: 'ts-loader',
                    },
                ],
            },
        ],
    },
};
