const path = require('path');

const HtmlWebPackPlugin = require('html-webpack-plugin');
const { TypedCssModulesPlugin } = require('typed-css-modules-webpack-plugin');

const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const MiniCssExtractPluginConfig = new MiniCssExtractPlugin({
    filename: 'smartplaylist.2.5.1.4830.css',
    chunkFilename: '[local].css',
});

const htmlWebpackPlugin = new HtmlWebPackPlugin({
    template: './index.html',
    filename: './index.html',
});

const outDir = path.join(__dirname, './dist');

module.exports = {
    mode: 'development',
    entry: ['./src/index'],

    devServer: {
        contentBase: outDir,
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
        filename: 'smartplaylist.2.5.1.4830.js',
    },

    plugins: [
        MiniCssExtractPluginConfig,
        htmlWebpackPlugin,
        new TypedCssModulesPlugin({
            globPattern: 'src/**/*.css',
        }),
    ],
    devServer: {
        hot: true,
        historyApiFallback: true,
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
                            modules: {
                                localIdentName: '[name]__[local]',
                            },
                            importLoaders: 1,
                            sourceMap: true,
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
